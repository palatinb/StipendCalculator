import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UniaddComponent } from './uniadd.component';

describe('UniaddComponent', () => {
  let component: UniaddComponent;
  let fixture: ComponentFixture<UniaddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UniaddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UniaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
