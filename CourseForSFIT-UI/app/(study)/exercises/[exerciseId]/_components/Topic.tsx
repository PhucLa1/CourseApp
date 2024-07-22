import { ContentExercise } from "@/model/Exercises"


export default function Topic({ content }: { content: ContentExercise }) {
    return (
        <div className='mt-3 ml-2 max-h-[650px] overflow-y-auto'>
            {content.description != "" && <div className="">
                <h2 className="text-[18px] font-bold">Mô tả bài toán</h2>
                <div className="text-sm text-gray-400 mt-1" dangerouslySetInnerHTML={{ __html: content.description }}></div>
            </div>}
            {content.constraints != "" && <div className="mt-3">
                <h2 className="text-[18px] font-bold">Ràng buộc bài toán</h2>
                <div className="text-sm text-gray-400 mt-1" dangerouslySetInnerHTML={{ __html: content.constraints }}></div>
            </div>}
            {content.inputFormat != "" && <div className="mt-3">
                <h2 className="text-[18px] font-bold">Định dạng đầu vào</h2>
                <div className="text-sm text-gray-400 mt-1" dangerouslySetInnerHTML={{ __html: content.inputFormat }}></div>
            </div>}
            {content.outputFormat != "" && <div className="mt-3">
                <h2 className="text-[18px] font-bold">Định dạng đầu ra</h2>
                <div className="text-sm text-gray-400 mt-1" dangerouslySetInnerHTML={{ __html: content.outputFormat }}></div>
            </div>}
            {
                content.output?.map((item, index) => {
                    return <div key={index}>
                        <div className="mt-3">
                            <h2 className="text-[18px] font-bold">Dữ liệu đầu vào</h2>
                            <div dangerouslySetInnerHTML={{ __html: content.input[index] }} className="pt-3 text-sm text-gray-400 h-[80px] border border-gray rounded-md mt-3 bg-gray-700 mr-2 pl-4 leading-normal">
                            </div>
                        </div>
                        <div className="mt-3">
                            <h2 className="text-[18px] font-bold">Dữ liệu đầu ra</h2>
                            <div dangerouslySetInnerHTML={{ __html: item }} className="pt-3 text-sm text-gray-400 h-[80px] border border-gray rounded-md mt-3 bg-gray-700 mr-2 pl-4 leading-normal">
                            </div>
                        </div>
                    </div>
                })
            }
            {content.explaintation != "" && <div className="mt-3 mb-3">
                <h2 className="text-[18px] font-bold">Giải thích</h2>
                <div className="text-sm text-gray-400 mt-1" dangerouslySetInnerHTML={{ __html: content.explaintation }}></div>
            </div>}
        </div >
    )
}
