import 'package:chatapp_frontend/core/common_widgets/app_scaffold.dart';
import 'package:chatapp_frontend/core/common_widgets/app_text_form_field.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:go_router/go_router.dart';

import '../blocs/auth_bloc.dart';

class LoginForm extends HookWidget {
  const LoginForm({super.key});

  @override
  Widget build(BuildContext context) {
    final formKey = useState(GlobalKey<FormState>());
    final emailController = useTextEditingController();
    final passwordController = useTextEditingController();
    return AppScaffold(
      title: 'Login',
      child: SingleChildScrollView(
        child: Center(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 32),
            child: Form(
              key: formKey.value,
              child: Column(
                spacing: 32,
                children: [
                  AppTextFormField(
                    controller: emailController,
                    width: double.infinity,
                    labelText: 'Email',
                    validator: (value) {
                      if (value == null || value.isEmpty) {
                        return 'Email must not be empty';
                      }
                      return null;
                    },
                  ),
                  AppTextFormField(
                    controller: passwordController,
                    width: double.infinity,
                    labelText: 'Password',
                    obscureText: true,
                    validator: (value) {
                      if (value == null || value.isEmpty) {
                        return 'Password must not be empty';
                      }
                      return null;
                    },
                  ),
                  SizedBox(
                    width: double.infinity,
                    height: 60,
                    child: ElevatedButton.icon(
                      icon: Icon(
                        Icons.navigate_next,
                        color: Theme.of(context).colorScheme.onSecondary,
                      ),
                      onPressed: () {
                        if (formKey.value.currentState!.validate()) {
                          context.read<AuthBloc>().add(
                                SignInWithEmail(
                                  email: emailController.text.trim(),
                                  password: passwordController.text.trim(),
                                ),
                              );
                        }
                      },
                      label: Text(
                        'Login',
                        style: TextStyle(
                          color: Theme.of(context).colorScheme.onSecondary,
                        ),
                      ),
                      style: ElevatedButton.styleFrom(
                        backgroundColor:
                            Theme.of(context).colorScheme.secondary,
                      ),
                    ),
                  ),
                  TextButton(
                    onPressed: () => context.push('/register'),
                    child: const Center(
                      child: Text('Not yet registered? Click here'),
                    ),
                  ),
                  const Center(child: Text('OR')),
                  SizedBox(
                    height: 60,
                    width: double.infinity,
                    child: ElevatedButton(
                      onPressed: () {
                        context.read<AuthBloc>().add(SignInWithGoogle());
                      },
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        spacing: 10,
                        children: [
                          Image.asset(
                            'assets/images/google_logo.svg',
                            height: 24,
                            width: 24,
                          ),
                          const Text('Sign in with Google'),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
