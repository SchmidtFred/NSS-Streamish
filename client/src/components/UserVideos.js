import React, { useEffect, useState } from "react";
import { Card, CardBody } from "reactstrap";
import { useParams } from "react-router-dom";
import Video from "./Video";
import { getUserWithVideos } from "../modules/videoManager";

const UserVideos = () => {
    const [user, setUser] = useState({});
    const { id } = useParams();

    useEffect(() => {
        getUserWithVideos(id).then(setUser);
    }, []);

    return (
        <div className="container">
            <Card>
                <p className="text-left px-2">{user.name}</p>
                <CardBody>
                    {user.imageUrl ? `<img src="${user.imageUrl} alt=${user.name} />` : null}
                </CardBody>
            </Card>
            {user.videos?.map(video => <Video video={video} key={video.id} />)}
        </div>
    );
};

export default UserVideos;