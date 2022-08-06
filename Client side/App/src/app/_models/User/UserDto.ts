export class UserDto {

  constructor(
    public id: number,
    public name: string,
    public username: string,
    public token: string,
    public roleId: number,
    public uid: number,
    public last_login: Date
  ) { }
}
