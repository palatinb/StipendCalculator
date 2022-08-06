import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TanulmanyiComponent } from './tanulmanyi.component';

describe('TanulmanyiComponent', () => {
  let component: TanulmanyiComponent;
  let fixture: ComponentFixture<TanulmanyiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TanulmanyiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TanulmanyiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
