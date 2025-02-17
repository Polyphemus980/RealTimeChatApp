class Result<T> {
  Result._({required this.isSuccess, this.errorMessage, this.data});

  factory Result.success(T data) => Result._(isSuccess: true, data: data);

  factory Result.failure(String errorMessage) =>
      Result._(isSuccess: false, errorMessage: errorMessage);

  final bool isSuccess;
  final String? errorMessage;
  final T? data;
}
