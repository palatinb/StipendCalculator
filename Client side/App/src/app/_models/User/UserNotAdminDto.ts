export class UserNotAdminDto {
    constructor(
        public id: number,
        public name: string,
        public username: string,
        public password: string,
        public passwordHash: string,
        public passwordHash2: string,
    ) {}
}