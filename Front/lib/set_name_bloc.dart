import 'package:bloc/bloc.dart';
import 'package:chatapp_frontend/user_service.dart';

sealed class SetNameEvent {}

class NameChanged extends SetNameEvent {
  NameChanged({required this.newName});

  final String newName;
}

sealed class SetNameState {}

class NoData extends SetNameState {}

class NameTaken extends SetNameState {}

class NameFree extends SetNameState {}

class SetNameBloc extends Bloc<SetNameEvent, SetNameState> {
  SetNameBloc({required UserApiService userApiService})
      : _userApiService = userApiService,
        super(NoData()) {
    on<NameChanged>(_checkIfNameFree);
  }
  final UserApiService _userApiService;

  Future<void> _checkIfNameFree(
    NameChanged event,
    Emitter<SetNameState> emit,
  ) async {
    final result = await _userApiService.checkIfNameFree(event.newName);
    if (result.isSuccess) {
      final isFree = result.data!;
      isFree ? emit(NameFree()) : emit(NameTaken());
    }
  }
}
