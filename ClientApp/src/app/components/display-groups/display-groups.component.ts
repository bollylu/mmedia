import { Component, OnInit } from '@angular/core';
import { IGroup } from '../../model/IGroup';
import { MoviesService } from '../../services/movies.service';

@Component({
  selector: 'app-display-groups',
  templateUrl: './display-groups.component.html',
  styleUrls: ['./display-groups.component.scss']
})

export class DisplayGroupsComponent implements OnInit {

  Groups: IGroup[];
  GroupsCount = 0;

  CurrentName = "Root";

  Level = 1;

  isDirty = true;

  isSelected(group: IGroup) {
    return group.isSelected;
  }

  constructor(private moviesService: MoviesService) { }

  ngOnInit(): void {
    // Init => Display first page of all data, both groups and movies
    this.CurrentName = "";
    this.isDirty = false;
    this.moviesService.getGroups(this.CurrentName).subscribe(g => {
      //console.log("ngInit : Got group : " + this.CurrentName);
      this.Groups = g.groups;
      this.GroupsCount = this.Groups.length;
      //console.log("ngInit => " + this.GroupsCount + " items");
      this.isDirty = true;
    });
  }

  // On group is selected => display only this group
  GroupSelected(group: IGroup) {
    console.log("Selected group : " + group.name);
    this.CurrentName = group.name;
    this.isDirty = false;
    if (this.GroupsCount > 0) {
      this.Level++;
      //console.log("Requesting subgroups for " + group.name + " level " + this.Level);
      this.moviesService.getGroups(group.name).subscribe(g => {
        //console.log("groupSelected : Got group : " + g.name);
        this.Groups = g.groups;
        this.GroupsCount = this.Groups.length;
        console.log(this.GroupsCount + " items");
        this.isDirty = true;
      });

    }
  }

  // Back button is pressed => go back one level
  goBack() {
    console.log("Going back");
    this.isDirty = false;
    if (this.Level > 1) {
      this.Level--;
      
      this.CurrentName = this.CurrentName.split("/").slice(0, this.Level - 1).join("/");
      this.moviesService.getGroups(this.CurrentName).subscribe(g => {
        //console.log("groupSelected : Got group : " + g.name);
        this.Groups = g.groups;
        this.GroupsCount = this.Groups.length;
        //console.log(this.GroupsCount + " items");
        this.isDirty = true;
      });
    }
  }

  //#region --- Display helper ----------------------------------------------
  displayTitle(group: string) {

    //console.log("===> displaying group title: " + group);
    if (group === "") {
      return "All items";
    }

    while (group.endsWith("/")) {
      group = group.substr(0, group.length - 1);
    }

    if (group.indexOf("/") < 0) {
      return group;
    }

    return group.split("/").slice(- 1).join("/");
  }

  isRoot() {
    return this.CurrentName === "";
  }
  //#endregion --- Display helper -------------------------------------------
}
