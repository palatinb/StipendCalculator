import { StudentCalculationDto } from '../Student/StudentCalculationDto';

//Ösztöndíjszámoláshoz szükséges adatokat tartalmazó object
export class CalculateDto {
  constructor(
    public maxPrice: number,
    public minPrice: number,
    public minStipendIndex: number,
    public input: number,
    public students: StudentCalculationDto[],
  ) {}
}
