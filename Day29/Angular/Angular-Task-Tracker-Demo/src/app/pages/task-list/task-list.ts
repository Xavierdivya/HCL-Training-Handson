import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Task } from '../../models/task';
import { TaskService } from '../../services/task.service';

@Component({
  selector:'app-task-list',
  standalone:true,
  imports:[FormsModule],
  templateUrl:'./task-list.html',
  styleUrl:'./task-list.css'
})

export class TaskList {
  tasks:Task[]=[];
  newTitle='';
  newCategory='Learning';

  constructor(private taskService:TaskService){
    this.loadTasks();
  }

  loadTasks():void{this.tasks=this.taskService.getTasks();}

  addTask():void{
    const title=this.newTitle.trim();
    if(!title)return;
    this.taskService.addTask(title,this.newCategory);
    this.newTitle='';this.newCategory='Learning';this.loadTasks();
  }
  toggleTask(id:number):void{this.taskService.toggleTask(id);this.loadTasks();}
  
  deleteTask(id:number):void{this.taskService.deleteTask(id);this.loadTasks();}
  get completedCount():number{return this.tasks.filter(task=>task.completed).length;}
}