import 'package:chatapp_frontend/features/theme/theme_notifier.dart';
import 'package:chatapp_frontend/get_it_di.dart';
import 'package:chatapp_frontend/routes.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'firebase_options.dart';
import 'global_providers.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );
  getItSetUp();
  runApp(
    const GlobalProviders(
      child: MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    final theme = Provider.of<ThemeNotifier>(context).theme;
    return MaterialApp.router(
      routerConfig: router,
      theme: theme,
    );
  }
}
