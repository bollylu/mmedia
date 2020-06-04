import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IGroup } from '../../model/IGroup';

@Component({
  selector: 'app-display-group',
  templateUrl: './display-group.component.html',
  styleUrls: ['./display-group.component.scss']
})
export class DisplayGroupComponent implements OnInit {

  constructor() { }

  @Input()
  Group: IGroup;

  @Output() groupSelected: EventEmitter<IGroup> = new EventEmitter<IGroup>();

  @Input()
  IsSelected: boolean;

  @Input()
  Level: number;

  DisplayName: string;

  ngOnInit(): void {
  }

  onGroupSelect() {
    console.log('selecting ' + this.Group.name);
    this.groupSelected.emit(this.Group);
    this.IsSelected = !this.IsSelected;
  }

  displayName() {
    if (this.Group.name == '') {
      return '---';
    }
    var values = this.Group.name.split('\\');
    var retVal = values.slice(this.Level-1);
    return retVal.join("/");
  }
}
