export class UserShowDto {

  constructor(
    public id: number,
    public name: string,
    public username: string,
    public roleName: number,
    public lastLogin: Date
  ) { }
}
