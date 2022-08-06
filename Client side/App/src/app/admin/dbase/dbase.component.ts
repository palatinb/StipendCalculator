import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../_services/student.service';
import { UploadDto } from '../../_models/Other/UploadDto';
import { UniService } from '../../_services/uni.service';
import { UniDto } from '../../_models/UniDto';
import alertify from 'alertifyjs';
import { Title } from '@angular/platform-browser';  

@Component({
  selector: 'app-dbase',
  templateUrl: './dbase.component.html',
  styleUrls: ['./dbase.component.css']
})
export class DbaseComponent implements OnInit {

  form: any;

  constructor(private uniService: UniService, private studentService: StudentService, private titleService: Title) {
    this.titleService.setTitle('Adatbázis feltöltése');
  }

  universities: UniDto[];
  fileToUpload: File;
  uploadModel = new UploadDto('0', null);
  formdata: FormData = new FormData();

  onFileChange(files: FileList) {
    if (files.length > 0) {
      this.fileToUpload = files.item(0);
    }
  }

  ngOnInit() {
    this.uniService.GetAllUni().subscribe(data => this.universities = data as UniDto[]);
  }
  onSubmit() {
    this.uploadModel.file = this.fileToUpload;
    this.formdata.append('uni', this.uploadModel.uni);
    this.formdata.append('file', this.uploadModel.file);
    if (this.uploadModel.file == null || this.uploadModel.uni == '') {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Helytelenül töltötted ki a  mezőket!', 3);
    } else {
      this.studentService.Upload(this.formdata).subscribe((result) => {
        alertify.set('notifier', 'position', 'top-right');
        alertify.success('Sikeresen feltöltöted az adatbázíst!', 3);
      },
        error => {
          alertify.set('notifier', 'position', 'top-right');
          alertify.error('Sikertelen feltőltés!', 3);          
        });
    }
  }

}
