//bejelentkezéshez szükséges object
export class UserLoginDto {

  constructor(
    public username: string,
    public password: string,
    //public Last_login: Date
  ) { }

}
