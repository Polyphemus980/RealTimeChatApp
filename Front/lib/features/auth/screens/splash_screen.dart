import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';

import '../blocs/auth_bloc.dart';

class SplashScreen extends StatelessWidget {
  const SplashScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocListener<AuthBloc, AuthState>(
      listener: (context, state) {
        if (state is SignedIn) {
          context.go('/signout');
          return;
        }
        if (state is SignedInNeedData) {
          context.go('/data');
          return;
        }
        if (state is PendingEmailVerification) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Please check your email to confirm registration'),
            ),
          );
        }
        context.go('/login');
      },
      child: const Center(
        child: FlutterLogo(size: 160),
      ),
    );
  }
}
