import 'package:chatapp_frontend/features/auth/blocs/auth_bloc.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

extension BuildContextExtensions on BuildContext {
  String get currentUserId {
    final authState = read<AuthBloc>().state;
    if (authState is! SignedIn) {
      throw StateError('No authenticated user found');
    }
    return authState.user.uid;
  }
}
