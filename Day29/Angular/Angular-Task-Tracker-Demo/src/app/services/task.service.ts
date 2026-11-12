import { Injectable } from '@angular/core';
import { Task } from '../models/task';

@Injectable({providedIn:'root'})

export class TaskService {
  private nextId=4;
  private tasks:Task[]=[
    {id:1,title:'Learn Angular components',category:'Learning',completed:true},
    {id:2,title:'Understand dependency injection',category:'Learning',completed:false},
    {id:3,title:'Build a small demo',category:'Practice',completed:false}
  ];

  getTasks():Task[]{return [...this.tasks];}
  
  addTask(title:string,category:string):void{
    this.tasks.push({id:this.nextId++,title,category,completed:false});
  }
  toggleTask(id:number):void{
    const task=this.tasks.find(item=>item.id===id);
    if(task) task.completed=!task.completed;
  }
  deleteTask(id:number):void{this.tasks=this.tasks.filter(task=>task.id!==id);}
}