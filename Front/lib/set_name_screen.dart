import 'package:chatapp_frontend/core/common_widgets/app_scaffold.dart';
import 'package:chatapp_frontend/core/common_widgets/app_text_form_field.dart';
import 'package:chatapp_frontend/features/auth/blocs/auth_bloc.dart';
import 'package:chatapp_frontend/set_name_bloc.dart';
import 'package:chatapp_frontend/user_service.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:go_router/go_router.dart';

import 'get_it_di.dart';

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
    return AppScaffold(
      title: 'Additional information',
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 32),
        child: Column(
          spacing: 32,
          children: [
            Form(
              key: formKey.value,
              child: AppTextFormField(
                controller: textController,
                width: double.infinity,
                labelText: 'Name',
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Name must not be empty';
                  }
                  return null;
                },
              ),
            ),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: () {},
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
    );
  }
}
