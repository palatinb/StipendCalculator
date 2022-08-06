import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OcsiMutatoComponent } from './ocsi-mutato.component';

describe('OcsiMutatoComponent', () => {
  let component: OcsiMutatoComponent;
  let fixture: ComponentFixture<OcsiMutatoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OcsiMutatoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OcsiMutatoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
