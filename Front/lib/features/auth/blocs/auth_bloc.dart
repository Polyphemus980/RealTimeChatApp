import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:chatapp_frontend/user_api_service.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:google_sign_in/google_sign_in.dart';

sealed class AuthEvent {}

class SignInWithGoogle extends AuthEvent {}

class SignInWithEmail extends AuthEvent {
  SignInWithEmail({required this.email, required this.password});
  final String email;
  final String password;
}

class SignUpWithEmail extends AuthEvent {
  SignUpWithEmail({required this.email, required this.password});
  final String email;
  final String password;
}

class ResendVerificationEmail extends AuthEvent {}

class EmailValidated extends AuthEvent {}

class SignOut extends AuthEvent {}

class RegistrationFinished extends AuthEvent {}

class AuthStateChanged extends AuthEvent {
  AuthStateChanged({required this.user, required this.token});
  final User? user;
  final String? token;
}

sealed class AuthState {}

class Loading extends AuthState {}

class SignedOut extends AuthState {}

class SignedIn extends AuthState {
  SignedIn({required this.user, required this.token});
  final User user;
  final String token;
}

class SignedInNeedData extends AuthState {
  SignedInNeedData({required this.user, required this.token});
  final User user;
  final String token;
}

class PendingEmailVerification extends AuthState {
  PendingEmailVerification({required this.user});
  final User user;
}

class AuthError extends AuthState {
  AuthError({required this.message});
  final String message;
}

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  AuthBloc({
    required FirebaseAuth authInstance,
    required UserApiService userApiService,
  })  : _authInstance = authInstance,
        _userApiService = userApiService,
        super(SignedOut()) {
    _handleAuthStateChanges();
    on<AuthStateChanged>(_handleStateChanged);
    on<SignInWithEmail>(_signInWithEmail);
    on<SignUpWithEmail>(_signUpWithEmail);
    on<ResendVerificationEmail>(_resendVerificationEmail);
    on<SignInWithGoogle>(_signInWithGoogle);
    on<RegistrationFinished>(_finishRegistration);
    on<SignOut>(_signOut);
  }
  final FirebaseAuth _authInstance;
  final UserApiService _userApiService;
  StreamSubscription<User?>? _authStateSubscription;

  void _handleAuthStateChanges() {
    _authStateSubscription = _authInstance.userChanges().listen((user) async {
      final token = await user?.getIdToken();
      add(AuthStateChanged(user: user, token: token));
    });
  }

  Future<void> _signInWithEmail(
    SignInWithEmail event,
    Emitter<AuthState> emit,
  ) async {
    try {
      await _authInstance.signInWithEmailAndPassword(
        email: event.email,
        password: event.password,
      );
    } catch (err) {
      emit(AuthError(message: err.toString()));
      emit(SignedOut());
    }
  }

  Future<void> _signUpWithEmail(
    SignUpWithEmail event,
    Emitter<AuthState> emit,
  ) async {
    try {
      final userCredentials =
          await _authInstance.createUserWithEmailAndPassword(
        email: event.email,
        password: event.password,
      );
      await userCredentials.user!.sendEmailVerification();
      emit(PendingEmailVerification(user: userCredentials.user!));
    } catch (err) {
      emit(AuthError(message: err.toString()));
      emit(SignedOut());
    }
  }

  Future<void> _resendVerificationEmail(
    ResendVerificationEmail event,
    Emitter<AuthState> emit,
  ) async {
    if (state is PendingEmailVerification) {
      final pendingVerificationState = state as PendingEmailVerification;
      await pendingVerificationState.user.sendEmailVerification();
    }
  }

  Future<void> _signInWithGoogle(
    SignInWithGoogle event,
    Emitter<AuthState> emit,
  ) async {
    try {
      final googleUser = await GoogleSignIn().signIn();

      final googleAuth = await googleUser!.authentication;

      final credential = GoogleAuthProvider.credential(
        accessToken: googleAuth.accessToken,
        idToken: googleAuth.idToken,
      );
      await FirebaseAuth.instance.signInWithCredential(credential);
    } catch (err) {
      emit(AuthError(message: err.toString()));
    }
  }

  Future<void> _signOut(SignOut event, Emitter<AuthState> emit) async {
    await _authInstance.signOut();
  }

  Future<void> _handleStateChanged(
    AuthStateChanged event,
    Emitter<AuthState> emit,
  ) async {
    _userApiService.updateToken(event.token);
    if (state is SignedIn && event.user != null) {
      return;
    }
    if (event.user != null) {
      if (!event.user!.emailVerified) {
        emit(PendingEmailVerification(user: event.user!));
        return;
      }
      final result = await _userApiService.checkIfNewUser();
      if (result.isSuccess) {
        final isNewUser = result.data!;
        !isNewUser
            ? emit(SignedIn(user: event.user!, token: event.token!))
            : emit(
                SignedInNeedData(user: event.user!, token: event.token!),
              );
      } else {
        emit(AuthError(message: 'Could not verify user'));
      }
      return;
    }
    emit(SignedOut());
  }

  void _finishRegistration(
    RegistrationFinished event,
    Emitter<AuthState> emit,
  ) {
    if (state is SignedInNeedData) {
      final currentState = state as SignedInNeedData;
      emit(
        SignedIn(user: currentState.user, token: currentState.token),
      );
    }
  }

  @override
  Future<void> close() async {
    await _authStateSubscription?.cancel();
    await super.close();
  }
}
