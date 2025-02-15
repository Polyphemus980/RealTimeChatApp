import 'package:chatapp_frontend/features/auth/screens/login_screen.dart';
import 'package:chatapp_frontend/features/auth/screens/sign_up_screen.dart';
import 'package:chatapp_frontend/features/auth/screens/splash_screen.dart';
import 'package:go_router/go_router.dart';

final router = GoRouter(
  initialLocation: '/splash',
  routes: [
    GoRoute(
      path: '/splash',
      builder: (context, state) => const SplashScreen(),
    ),
    GoRoute(
      path: '/login',
      builder: (context, state) => const LoginScreen(),
    ),
    GoRoute(
      path: '/register',
      builder: (context, state) => const SignUpScreen(),
    ),
  ],
);
