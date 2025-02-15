import 'package:flutter/material.dart';

class ThemeNotifier extends ChangeNotifier {
  ThemeData _theme =
      ThemeMode.system == ThemeMode.dark ? ThemeData.dark() : ThemeData.light();
  ThemeData get theme => _theme;

  void toggleTheme() {
    _theme = _theme == ThemeData.dark() ? ThemeData.light() : ThemeData.dark();
    notifyListeners();
  }
}
