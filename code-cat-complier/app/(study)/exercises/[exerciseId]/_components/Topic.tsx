

export default function Topic({ content} : {content: string}) {
    console.log(content)
    return (
        <div dangerouslySetInnerHTML={{__html: content}} className='mt-3 ml-2 max-h-[650px] overflow-y-auto'>
            
        </div>
    )
}
