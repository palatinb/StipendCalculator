export class UserModifiedDto {
    constructor(
        public id:number,
        public name: string,
        public username: string,
        public passwordHash: string,
        public roleId: number,
        public uiD: number,
    ) {}
}
