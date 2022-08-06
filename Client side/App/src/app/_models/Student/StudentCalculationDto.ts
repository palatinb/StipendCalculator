//az ösztöndíjszámolásnál megjelenített oszlopok
export class StudentCalculationDto {
  constructor(
    public neptunCode: string,
    public stipendIndex: string,
    public groupIndex: string,
    public stipendAmmount: number,
  ) {}
}
