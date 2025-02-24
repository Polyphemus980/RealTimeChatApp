import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class AppTextFormField extends StatelessWidget {
  const AppTextFormField({
    super.key,
    this.errorStyle,
    this.labelText,
    this.hintText,
    this.formatters,
    this.controller,
    this.width,
    this.height,
    this.onChanged,
    this.onEditingComplete,
    this.errorText,
    this.validator,
    this.keyboardType,
    this.maxLines,
    this.obscureText = false,
    this.readOnly = false,
  });
  final List<TextInputFormatter>? formatters;
  final TextEditingController? controller;
  final double? height;
  final double? width;
  final String? labelText;
  final String? hintText;
  final String? errorText;
  final bool obscureText;
  final bool readOnly;
  final int? maxLines;
  final TextStyle? errorStyle;
  final String? Function(String?)? validator;
  final void Function()? onEditingComplete;
  final void Function(String)? onChanged;

  final TextInputType? keyboardType;

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: width ?? 80,
      height: height,
      child: TextFormField(
        validator: validator,
        onEditingComplete: onEditingComplete,
        inputFormatters: formatters,
        controller: controller,
        style: Theme.of(context).textTheme.titleMedium,
        keyboardType: keyboardType,
        onChanged: onChanged,
        obscureText: obscureText,
        maxLines: obscureText ? 1 : maxLines,
        readOnly: readOnly,
        decoration: InputDecoration(
          hintText: hintText,
          labelText: labelText,
          errorText: errorText,
          errorStyle: errorStyle,
          contentPadding: const EdgeInsets.fromLTRB(8, 8, 0, 8),
          filled: true,
          fillColor: Theme.of(context).colorScheme.primaryContainer,
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(10),
            borderSide: BorderSide(
              color: Theme.of(context).colorScheme.secondary,
              width: 2,
            ),
          ),
          focusedBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(10),
            borderSide: const BorderSide(
              color: Colors.blueAccent,
              width: 2,
            ),
          ),
        ),
      ),
    );
  }
}
