import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { TaskList } from './pages/task-list/task-list';
export const routes: Routes = [
  {path:'',component:Home,pathMatch:'full'},
  {path:'tasks',component:TaskList},
  {path:'**',redirectTo:''}
];