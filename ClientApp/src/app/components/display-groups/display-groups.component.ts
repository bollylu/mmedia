import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IGroup } from '../../model/IGroup';
import { MoviesService } from '../../services/movies.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-display-groups',
  templateUrl: './display-groups.component.html',
  styleUrls: ['./display-groups.component.scss']
})

export class DisplayGroupsComponent implements OnInit {

  Groups$: Observable<IGroup[]>;

  @Input()
  SelectedGroupName: string;

  Level: number = 1;

  constructor(private moviesService: MoviesService) { }

  ngOnInit(): void {
    this.Groups$ = this.moviesService.getGroups('', 1);
  }

  GroupSelected(group: IGroup) {
    this.SelectedGroupName = group.name;
    this.Level++;
    this.Groups$ = this.moviesService.getGroups(this.SelectedGroupName, this.Level);
  }

  goBack() {
    this.Level--;
    if (this.Level == 1) {
      this.SelectedGroupName = '';
    }
    this.Groups$ = this.moviesService.getGroups(this.SelectedGroupName, this.Level);
  }

  displayTitle(group: string, level: number) {
    if (group == "") {
      return "---";
    }
    if (group.indexOf("/") == 0) {
      return group;
    }
    var values = group.split("/");
    var retVal = values.slice(level - 1);
    return retVal.join("/");
  }
}
