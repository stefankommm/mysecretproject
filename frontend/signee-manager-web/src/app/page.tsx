import UserAuth from "@/components/modules/auth/user-auth"

const url = "http://localhost:5140"; // TODO setup somehow BackendURL

const Home = () => {
    return (
        <div className="h-screen w-screen bg-zinc-900 flex items-center justify-center">
            <div className="min-h-[520px]">
                <div className="w-[320px] md:w-[430px] bg-white rounded-md p-4 lg:p-8">
                    <div className="z-20 pb-4 lg:pb-6 text-white font-bold text-2xl text-stroke">
                        SIGNEE
                    </div>
                    <UserAuth url={url}/>
                </div>
            </div>
        </div>
    );
}

export default Home;
