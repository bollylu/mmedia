import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayGroupComponent } from './display-group.component';

describe('DisplayGroupComponent', () => {
  let component: DisplayGroupComponent;
  let fixture: ComponentFixture<DisplayGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DisplayGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DisplayGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
