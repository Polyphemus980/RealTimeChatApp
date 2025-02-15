import 'package:chatapp_frontend/core/common_widgets/app_scaffold.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';

import '../blocs/auth_bloc.dart';
import '../widgets/sign_up_form.dart';

class SignUpScreen extends StatelessWidget {
  const SignUpScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<AuthBloc, AuthState>(
      builder: (context, state) {
        if (state is Loading) {
          return const AppScaffold(
            title: 'Sign up',
            child: Center(child: CircularProgressIndicator()),
          );
        }
        return const SignUpForm();
      },
      listener: (context, state) {
        if (state is AuthError) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content:
                  Text('Error occurred while authenticating: ${state.message}'),
            ),
          );
        } else if (state is PendingEmailVerification) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text(
                'Sign-up successful! Please check your email to confirm.',
              ),
            ),
          );
          context.go('/login');
        }
      },
    );
  }
}
