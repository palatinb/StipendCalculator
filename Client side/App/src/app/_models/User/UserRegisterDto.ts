// user regisztráláshoz szükséges object
export class UserRegisterDto {
  constructor(
    public Name: string,
    public Username: string,
    public PasswordHash: string,
    public RoleId: number,
    public UiD: number,
  ) { }

}
