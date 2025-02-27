import 'package:chatapp_frontend/features/auth/screens/login_screen.dart';
import 'package:chatapp_frontend/features/auth/screens/set_name_screen.dart';
import 'package:chatapp_frontend/features/auth/screens/sign_up_screen.dart';
import 'package:chatapp_frontend/features/auth/screens/splash_screen.dart';
import 'package:chatapp_frontend/features/chat/conversation_list/screens/conversation_list_screen.dart';
import 'package:chatapp_frontend/home_screen.dart';
import 'package:go_router/go_router.dart';
import 'package:provider/provider.dart';

import 'features/auth/blocs/auth_bloc.dart';
import 'features/chat/conversation/screens/conversation_screen.dart';

final router = GoRouter(
  redirect: (context, state) {
    final publicRoutes = ['/register', '/splash', '/data', '/login'];
    final authState = context.read<AuthBloc>();
    final isLoggedIn = authState is SignedIn;
    if (isLoggedIn && publicRoutes.contains(state.uri.toString())) {
      return '/home';
    } else if (!isLoggedIn && !publicRoutes.contains(state.uri.toString())) {
      return '/splash';
    } else {
      return null;
    }
  },
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
    GoRoute(
      path: '/data',
      builder: (context, state) => const SetNameScreen(),
    ),
    GoRoute(
      path: '/conversations',
      builder: (context, state) => const ConversationListScreen(),
      routes: [
        GoRoute(
          path: ':id',
          builder: (context, state) {
            final id = int.parse(state.pathParameters['id']!);
            return ConversationScreen(
              conversationId: id,
            );
          },
        ),
      ],
    ),
    GoRoute(
      path: '/home',
      builder: (context, state) => const HomeScreen(),
    ),
  ],
);
