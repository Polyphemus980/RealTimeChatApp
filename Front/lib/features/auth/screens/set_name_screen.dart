import 'package:chatapp_frontend/core/api/user_api_service.dart';
import 'package:chatapp_frontend/core/common/widgets/app_scaffold.dart';
import 'package:chatapp_frontend/core/common/widgets/app_text_form_field.dart';
import 'package:chatapp_frontend/features/auth/blocs/auth_bloc.dart';
import 'package:chatapp_frontend/features/auth/blocs/set_name_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:go_router/go_router.dart';

import '../../../core/dependency_injection/get_it_di.dart';

class SetNameScreen extends StatelessWidget {
  const SetNameScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider<SetNameBloc>(
      create: (context) => SetNameBloc(
        userApiService: getIt.get<UserApiService>(),
      ),
      child: const SetNameForm(),
    );
  }
}

class SetNameForm extends HookWidget {
  const SetNameForm({super.key});

  @override
  Widget build(BuildContext context) {
    final textController = useTextEditingController();
    final formKey = useState(GlobalKey<FormState>());
    return BlocListener<SetNameBloc, SetNameState>(
      listener: (context, state) {
        if (state is RegisterError) {
          textController.clear();
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Error occurred while trying to register user: ${state.errorMessage}',
              ),
            ),
          );
        } else if (state is RegisterSuccess) {
          context.read<AuthBloc>().add(RegistrationFinished());
          context.go('/home');
        }
      },
      child: AppScaffold(
        title: 'Additional information',
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 32),
          child: Column(
            spacing: 32,
            children: [
              Form(
                key: formKey.value,
                child: BlocBuilder<SetNameBloc, SetNameState>(
                  builder: (context, state) {
                    return AppTextFormField(
                      controller: textController,
                      width: double.infinity,
                      labelText: 'Name',
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Name must not be empty';
                        }
                        return null;
                      },
                      errorText: (state is NameTaken)
                          ? '${textController.text} is taken'
                          : null,
                      onChanged: (value) {
                        context.read<SetNameBloc>().add(
                              NameChanged(newName: value),
                            );
                      },
                    );
                  },
                ),
              ),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: () {
                    context.read<SetNameBloc>().add(
                          FinishedRegistering(newName: textController.text),
                        );
                  },
                  child: const Text('Finish'),
                ),
              ),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: () {
                    context.read<AuthBloc>().add(SignOut());
                    context.go('/login');
                  },
                  child: const Text('Sign out'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
