import { useCallback } from 'react'
import { useNotify } from 'react-admin'

const MAX_FILE_SIZE = 150000
const ALLOWED_FILE_TYPES = ['image/png', 'image/jpeg', 'image/gif']

export const useTransformImageFile = () => {
    const notify = useNotify()

    const transformImage = useCallback(
        (e) => {
            // Extract the file
            const files = Array.from(e.target.files)
            const file = files[0]

            // Validate file type
            if (ALLOWED_FILE_TYPES.every((type) => file.type !== type)) {
                notify(`'${file.type}' file type is not a supported format`, 'warning')
                return
            }

            // Validate file size
            if (file.size > MAX_FILE_SIZE) {
                notify(`File is too large, please pick a smaller file (limit 15mb)`, 'warning')
                return
            }

            return raTransformFileOnUpload(file)
        },
        [notify]
    )

    return transformImage
}

const raTransformFileOnUpload = (file) => {
    if (!(file instanceof File)) {
        return file
    }

    const preview = URL.createObjectURL(file)
    const transformedFile = {
        rawFile: file,
        url: preview,
        title: file.name,
    }

    return transformedFile
}