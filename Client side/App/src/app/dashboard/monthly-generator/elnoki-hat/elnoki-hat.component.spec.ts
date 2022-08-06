import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ElnokiHatComponent } from './elnoki-hat.component';

describe('ElnokiHatComponent', () => {
  let component: ElnokiHatComponent;
  let fixture: ComponentFixture<ElnokiHatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ElnokiHatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ElnokiHatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
