//Student keresénél a hallgatók leszűréséhez szükséges adatok
export class FilterStudentDto {
    constructor(
        public roleid: number,
        public uniid: number,
    ) {}

}