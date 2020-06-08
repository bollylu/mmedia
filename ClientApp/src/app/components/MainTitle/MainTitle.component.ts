import { Component, OnInit, Input } from '@angular/core';



@Component({
  selector: 'app-MainTitle',
  templateUrl: './MainTitle.component.html',
  styleUrls: ['./MainTitle.component.scss']
})



export class MainTitleComponent  {

  @Input()
  Title = 'Liste des vidéos';

  constructor() { }

  ngOnInit(): void {
    
  }

}


