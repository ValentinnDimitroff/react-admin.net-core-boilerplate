import React, { forwardRef, Fragment } from 'react'
import PropTypes from 'prop-types'
import { useTranslate } from 'react-admin'
import Button from '@material-ui/core/Button'
import { useTransformImageFile } from '../hooks'
import { convertFileToBase64 } from '../../../ra-providers/dataProvider-utils/images-handler'

const UploadPictureButton = forwardRef((
    {
        className,
        icon,
        label = 'Upload picture',
        onUpload = () => { },
    },
    ref
) => {
    const translate = useTranslate()
    const transformImage = useTransformImageFile()

    const handleOnUpdate = async (e) => {
        const fileToUpload = transformImage(e)

        if (fileToUpload) {
            const base64Image = await convertFileToBase64(fileToUpload)
            onUpload(base64Image);
        }
    }

    return (
        <Fragment>
            <input
                id="upload-picture"
                accept="image/*"
                type="file"
                style={{ display: 'none' }}
                onChange={(e) => handleOnUpdate(e)}
            />
            <label htmlFor="upload-picture" className={className}>
                <Button ref={ref} color="primary" variant="text" fullWidth startIcon={icon} component="span">
                    {translate(label)}
                </Button>
            </label>
        </Fragment>
    )
})

UploadPictureButton.propTypes = {
    className: PropTypes.string,
    icon: PropTypes.element,
    label: PropTypes.string,
    onUpload: PropTypes.func
}

export default UploadPictureButton