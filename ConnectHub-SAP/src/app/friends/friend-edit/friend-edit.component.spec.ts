/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FriendEditComponent } from './friend-edit.component';

describe('FriendEditComponent', () => {
  let component: FriendEditComponent;
  let fixture: ComponentFixture<FriendEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FriendEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FriendEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
