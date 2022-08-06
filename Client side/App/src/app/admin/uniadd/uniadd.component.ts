import { Component, OnInit } from '@angular/core';
import { UniService } from '../../_services/uni.service';
import { UniDto } from 'src/app/_models/UniDto';
import alertify from 'alertifyjs';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-uniadd',
  templateUrl: './uniadd.component.html',
  styleUrls: ['./uniadd.component.css']
})
export class UniaddComponent implements OnInit {

  uniModel = new UniDto(0, '');
  universities: UniDto[];
  displayedColumns: string[] = ['university', 'Delete'];
  noData = false;
  noNetwork = true;

  constructor(private uniService: UniService, private titleService: Title) {
    this.titleService.setTitle('Egyetem hozzáadása');
    //console.log(this.universities);
  }

  ngOnInit() {
    if (!navigator.onLine) {
      this.noNetwork = false;
      this.noData = true;
    } else {
      this.uniService.GetAllUni().subscribe(
        data => {
          this.universities = data as UniDto[];
          this.noData = true;
        }
      );
    }
  }

  trackBy(index, item) {
    return index;
  }

  DeleteUni(university) {
    this.uniService.DeleteUni(university.id).subscribe((result) => {
      alertify.set('notifier', 'position', 'top-right');
      alertify.success('Sikeresen törölted!', 3);
      this.uniService.GetAllUni().subscribe(data => this.universities = data as UniDto[]);
    },
      error => {
        alertify.set('notifier', 'position', 'top-right');
        alertify.success('Nem sikerült törörlni!', 3);
      });
  }

  onSubmit() {
    if (this.uniModel.name === '') {
      alertify.set('notifier', 'position', 'top-right');
      alertify.error('Helytelenül töltötted ki a  mezőket!', 3);
    } else {
      this.uniService.AddUni(this.uniModel)
        .subscribe(
          data => {
            this.uniService.GetAllUni().subscribe(dat => this.universities = dat as UniDto[]);
            this.uniModel.name = null;
            alertify.set('notifier', 'position', 'top-right');
            alertify.success('Siekeresen hozzáadtad!', 3);
          },
          error => {
            alertify.set('notifier', 'position', 'top-right');
            alertify.error('Siekertelen hozzáadás!', 3);
          });
    }
  }
}
