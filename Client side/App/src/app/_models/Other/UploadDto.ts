//fájlfeltöltéshez szükséges transfer object
export class UploadDto {
  constructor(
    public uni: string,
    public file: File,
  ) { }
}
