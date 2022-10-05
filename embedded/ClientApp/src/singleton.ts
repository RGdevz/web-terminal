
import * as signalR from '@microsoft/signalr/dist/esm'
import EventEmitter from "eventemitter3";





export function base_path():string{
	//@ts-ignore
return import.meta.env.VITE_CSHARP_ADDRESS
}

export const ismobile =  matchMedia('(hover: none)').matches;

export class singleton{



	private static _instance: singleton;
	private socket_connection:signalR.HubConnection
	public mitter = new EventEmitter()



protected 	constructor() {


	}






	 private async create_connection() {




		if (!this.socket_connection){


		this.socket_connection = new signalR.HubConnectionBuilder().withUrl(`${base_path()}/websocket`).withAutomaticReconnect().build();


		this.socket_connection.onreconnecting((err)=>{
	 this.mitter.emit('err-toast',err ?? 'error connecting to websocket')
		}
		)


		this.socket_connection.on('shell_session',(args => {
		this.mitter.emit('shell_session',args)
		}
		)
		)


	 await 	this.socket_connection.start()
		}


 	}






  	public async socket_send(method:string, ...args:string[]){

	 	try {

			if (!this.socket_connection){
			await this.create_connection()
			}

			if (this.socket_connection.state != 'Connected'){
			throw new Error('socket not connected')
			}

			return await this.socket_connection.invoke(method, ...args)

	 	}catch (e) {
	 	this.mitter.emit('err-toast',e.message ?? e)
			console.error(e.message ?? e)
	 	}

		 }





  	public static get Instance()
	 {

		if (!this._instance){
	 this._instance = new this()

		}

		return this._instance
	 }

 }