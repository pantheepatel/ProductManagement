import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, NgbCollapseModule, NgbDropdownModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'ProductAppFE';
  isMenuCollapsed = true;
  customerId: string | null = '89437jdshf89437';
}
