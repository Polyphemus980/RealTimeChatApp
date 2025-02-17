import 'package:chatapp_frontend/features/auth/blocs/auth_bloc.dart';
import 'package:chatapp_frontend/features/theme/theme_notifier.dart';
import 'package:chatapp_frontend/user_service.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:provider/provider.dart';

import 'get_it_di.dart';

class GlobalProviders extends StatelessWidget {
  const GlobalProviders({super.key, required this.child});

  final Widget child;
  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(
          create: (_) => ThemeNotifier(),
        ),
        BlocProvider<AuthBloc>(
          create: (_) => AuthBloc(
            authInstance: FirebaseAuth.instance,
            userApiService: getIt.get<UserApiService>(),
          ),
        ),
      ],
      child: child,
    );
  }
}
