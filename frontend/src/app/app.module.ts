import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import {
  BarController,
  BarElement,
  PieController,
  ArcElement,
  Chart,
  LineElement,
  LineController,
  CategoryScale,
  LinearScale,
  Title,
  Tooltip,
  Legend, PointElement,
} from 'chart.js';
import {provideHttpClient} from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';


Chart.register(PointElement, LineElement, LineController, ArcElement, PieController, BarController, BarElement, CategoryScale, LinearScale, Title, Tooltip, Legend);


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HomeComponent,
  ],
  providers: [provideHttpClient(),provideToastr(),provideAnimations()],
  bootstrap: [AppComponent]
})
export class AppModule { }
