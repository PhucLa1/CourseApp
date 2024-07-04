"use server"
import axios from "axios"
export async function compileCode(requestData:any){
    const endpoint = "https://emkc.org/api/v2/piston/execute"
    try {
        const reponse = await axios.post(endpoint, requestData)
        console.log("Response: ", reponse.data)
        return reponse.data
    } catch (error) {
        console.log(error)
        return error
    }
}