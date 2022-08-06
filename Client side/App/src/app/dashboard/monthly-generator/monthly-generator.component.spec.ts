import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyGeneratorComponent } from './monthly-generator.component';

describe('MonthlyGeneratorComponent', () => {
  let component: MonthlyGeneratorComponent;
  let fixture: ComponentFixture<MonthlyGeneratorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MonthlyGeneratorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MonthlyGeneratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
