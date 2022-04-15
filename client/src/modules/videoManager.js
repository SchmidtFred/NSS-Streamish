import { getToken } from "./authManager";
const baseUrl = '/api/video';

export const getAllVideos = () => {
    return getToken().then(token => {
        return fetch(`${baseUrl}/GetWithComments`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            }
        }).then(res => {
            if (res.ok) {
                return res.json();
            } else {
                throw new Error("An unknown error occurred while trying to get videos.");
            }
        });
    });
};

// export const getAllVideos = () => {
//     return fetch(`${baseUrl}/GetWithComments`).then(res => res.json());
// }

export const searchVideos = (searchTerm, sortDesc) => {
    return getToken().then(token => {
        return fetch(`${baseUrl}/search?q=${searchTerm}&sortDesc=${sortDesc}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            }
        }).then(res => {
            if (res.ok) {
                return res.json();
            } else {
                throw new Error("An unknown error occurred while trying to get videos.");
            }
        });
    });
};

export const addVideo = (video) => {
    return getToken().then(token => {
        return fetch(baseUrl, {
            method: "POST",
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify(video),
        }).then(res => {
            if (res.ok) {
                return res.json();
            } else if (res.status === 401) {
              throw new Error("unauthorized");  
            } else {
                throw new Error("An unknown error occurred while trying to upload your video.");
            }
        });
    });
};

export const getVideo = (id) => {
    return getToken().then(token => {
        return fetch(`${baseUrl}/GetWithComments/${id}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`
            }
        }).then(res => {
            if (res.ok) {
                return res.json();
            } else {
                throw new Error("An unkown error occurred while trying to get your video.");
            }
        });
    });
};

export const getUserWithVideos = (id) => {
    return getToken().then(token => {
        return fetch(`/api/UserProfile/GetProfileWithVideos/${id}`, {
            method: "Get",
            headers: {
                Authorization: `Bearer ${token}`
            }
        }).then(res => {
            if (res.ok) {
                return res.json();
            } else {
                throw new Error("An unknown error occurred while trying to get your video.");
            }
        });
    });
};