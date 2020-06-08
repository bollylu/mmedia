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

  @Input()
  IsSelected: boolean;

  @Input()
  Level: number;

  @Output() groupSelected: EventEmitter<IGroup> = new EventEmitter<IGroup>();

  DisplayName: string;

  ngOnInit(): void {
    //console.log("Group => " + this.Group.name);
  }

  onGroupSelect() {
    console.log('selecting ' + this.Group.name);
    this.groupSelected.emit(this.Group);
    this.IsSelected = true;
  }

  displayTitle(group: string) {
    //console.log("displaying : " + group + " level : " + level.toString());
    if (group === "") {
      return "---";
    }
    if (group.indexOf("/") < 0) {
      return group;
    }

    return group.split("/").slice(-1).join("/");
  }
}
