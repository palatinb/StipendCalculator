import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DbaseComponent } from './dbase.component';

describe('DbaseComponent', () => {
  let component: DbaseComponent;
  let fixture: ComponentFixture<DbaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DbaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DbaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
