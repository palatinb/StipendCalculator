//Generáláshoz szükséges összes adat elküldéséhez
export class GenerateDto{
    constructor(
        public Tan_Temanumber: string,
        public Koz_Temanumber: string,
        public FunkcioTerulet: string,
        public ETPay:string,

        public PresidentName: string,
        public PresidentNeptun: string,
        public VicePresidentName: string,
        public VicePresidentNeptun: string,
        public VicePresidentPercent: string,
        public EfName: string,
        public EfNeptun: string,
        public EfPercent: string,
        public GazdName: string,
        public GazdNeptun: string,
        public GazdPercent: string,
        public PrName: string,
        public PrNeptun: string,
        public PrPercent: string,
        public Ef2_Name: string,
        public Ef2_Neptun: string,
        public Ef2Percent: string,
        public Month: string,
        //szerver oldalon ki tudjam venni az adott kar (jogosult) hallgatóit
        public RoleId: string,
        public Uniid:string,
        public SemesterType: string,
        public faculty: string,

        public ElnokiIktato: string,
        public Tan_summaryIktatoSzam: string,
        public Tan_BizonylatIkatoszam: string,
        // közéleti ösztöndíj
        public file: File,
        public Kozeleti_BizonylatIktatoszam: string,
        public Kozeleti_SummaryIktatoszam: string,
    ) { }
}